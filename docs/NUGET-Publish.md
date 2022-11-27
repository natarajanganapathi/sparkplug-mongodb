# Nuget Publish Process

1. License file and README file is mandatory.
2. Upload directly into nuget for first time to understand the issue better. 
3. Generate the Token in API Token page
4. Use nuget command to push the package

```sh
dotnet nuget push *.nupkg -s https://api.nuget.org/v3/index.json -k ${{secrets.NUGET_API_KEY}} --skip-duplicate --no-symbols
```

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <AssemblyVersion>0.0.0.1</AssemblyVersion>
    <FileVersion>0.0.0.1</FileVersion>
    <Version>0.0.1-alpha1</Version>
    <PackageReadmeFile>README.md</PackageReadmeFile>
    <PackageLicenseFile>LICENSE.txt</PackageLicenseFile>
  </PropertyGroup>
  <ItemGroup>
    <!-- Your Dependency Packages list  -->
  </ItemGroup>
  <ItemGroup>
      <None Include="README.md" Pack="true" PackagePath="README.md"/>
      <None Include="LICENSE.txt" Pack="true" PackagePath="LICENSE.txt"/>
  </ItemGroup>
</Project>


```