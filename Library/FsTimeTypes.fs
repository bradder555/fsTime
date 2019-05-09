module TimeTypes
open System
open System.Globalization
open Hopac
open Hopac.Infixes

// this is number of nanoseconds since the unix epoc
// this should be it's own library
type DateTime' = | DateTime'' of uint64
module DateTime' =
  let private unpack (dt : DateTime') : uint64 =
    (dt |> function | DateTime'' x -> x)

  let private pack (ui : uint64) : DateTime' =
    ui |> DateTime''

  let private flip fn a b = fn b a
  let inline private (/.) a b = b / a

  let private msEpoc () =
    DateTime(1970,1,1,0,0,0,0,DateTimeKind.Utc)

  let private msDtNow () =
    job { return DateTime.UtcNow }

  let FromMsDt (msdt : DateTime) : DateTime' =
    let diff = msdt - msEpoc()
    (diff.Ticks |> uint64 ) * 100UL |> DateTime'' // convert to nano

  let FromNano (u : uint64) = u |> pack
  let FromMicro (u : uint64) = u * 1000UL |> pack
  let FromMilli (u : uint64) = u * 1000UL |> FromMicro
  let FromUnix (u : uint64) = u * 1000UL |> FromMilli

  let Now () =
    (msDtNow ()) >>- FromMsDt

  let ToISO (dt : DateTime') =
    let t =
      unpack dt
      |> (/.) 100UL
      |> int64  // convert to Ticks

    let ts = TimeSpan.FromTicks(t)
    let msdt = (msEpoc()) + ts
    msdt.ToString("o")

  let ToMicro (dt : DateTime') =
    unpack dt
    |> (/.) 1000UL // nano -> micro

  let ToMilli (dt : DateTime') =
    dt
    |> ToMicro
    |> (/.) 1000UL

  let ToUnix (dt : DateTime') =
    dt
    |> ToMilli
    |> (/.) 1000UL // micro -> seconds

  // we only attempt to parse Iso 8601 time, nothing else
  let TryParseIso (s : string) =
    let t =
      DateTime.TryParse
        (
          s,
          CultureInfo.InvariantCulture,
          DateTimeStyles.RoundtripKind
        )
    match t with
    | true, x -> Some x
    | false, _ -> None
    |> Option.map FromMsDt
