@echo off

cd APIDocumentation

dotnet clean
dotnet build -c Mvc3 -v m
dotnet build -c Mvc4 -v m
dotnet build -c Mvc5 -v m
dotnet build -c Mvc51 -v m

if ERRORLEVEL 1 goto eof

cd ../
nuget pack "NuSpec Files\SwaggerApiDocumentation.Mvc3.nuspec"
nuget pack "NuSpec Files\SwaggerApiDocumentation.Mvc4.nuspec"
nuget pack "NuSpec Files\SwaggerApiDocumentation.Mvc5.nuspec"
nuget pack "NuSpec Files\SwaggerApiDocumentation.Mvc51.nuspec"

:eof