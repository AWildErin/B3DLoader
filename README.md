# B3DLoader
A .b3d model parser for C#.

Does **not** support animations and bones currently.

Usage
```cs
var mdl = new B3DModel();
// Returns whether or not the read was successful
var result = mdl.ReadFromPath( filePath );
```


