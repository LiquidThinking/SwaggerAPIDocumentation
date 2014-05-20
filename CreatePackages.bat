@echo off
"%systemroot%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe" "APIDocumentation\SwaggerAPIDocumentation.csproj" /v:normal /t:Rebuild /p:Configuration=Mvc3
"%systemroot%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe" "APIDocumentation\SwaggerAPIDocumentation.csproj" /v:normal /t:Rebuild /p:Configuration=Mvc4
"%systemroot%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe" "APIDocumentation\SwaggerAPIDocumentation.csproj" /v:normal /t:Rebuild /p:Configuration=Mvc5
"%systemroot%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe" "APIDocumentation\SwaggerAPIDocumentation.csproj" /v:normal /t:Rebuild /p:Configuration=Mvc51

if ERRORLEVEL 1 goto eof

nuget pack "NuSpec Files\SwaggerApiDocumentation.Mvc3.nuspec"
nuget pack "NuSpec Files\SwaggerApiDocumentation.Mvc4.nuspec"
nuget pack "NuSpec Files\SwaggerApiDocumentation.Mvc5.nuspec"
nuget pack "NuSpec Files\SwaggerApiDocumentation.Mvc51.nuspec"

:eof