# Introduction to FsTime
*FsTime is a highly-opionated time library*

FsTime does not permit local time, by not restricting everything to universal time the library is greatly simplified.

If the end user wants to present local time, they can handle this in their presentation or view layer.

This simplifies the coding in the back-end, as no thought about converting between local and universal time is required.

The library stores time in a 64-bit uint, at nano second resolution, from epoch time.
(a limitation of this library is that it doesn't work prior to unix epoch)

On sixty-four bit architecture, this is the most-light weight representation of time

Time is covered in both the extremes of the very long and the very short.

