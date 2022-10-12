$file = [xml](Get-Content "C:\Users\F12\source\repos\SharedList\SharedList\SharedListApi\SharedListApi.csproj")
Write-Output $file.Project.PropertyGroup.Version
$breakingChange = Read-Host "Breaking change :Yes(y) or No(n)?"


if(($breakingChange.ToLower() -eq "y") -or ($breakingChange.ToLower() -eq "yes"))

{
    $file.Project.PropertyGroup.VersionMajor = [int]$file.Project.PropertyGroup.VersionMajor + 1
    $file.Project.PropertyGroup.VersionMinor = 0
    $file.Project.PropertyGroup.VersionPatch = 0
    $file.Save("C:\Users\F12\source\repos\SharedList\SharedList\SharedListApi\SharedListApi.csproj")
    exit 1
}
else{
    $commit = (git log -1 --pretty=%s)
    $commitType = $commit.Substring(0, $commit.IndexOf("("))
    if($commitType -eq "feat")
    {
        $file.Project.PropertyGroup.VersionMinor = [int]$file.Project.PropertyGroup.VersionMinor + 1
        $file.Project.PropertyGroup.VersionPatch = 0
        $file.Save("C:\Users\F12\source\repos\SharedList\SharedList\SharedListModels\SharedListModels.csproj")
        exit 1
    }
    else{
        $file.Project.PropertyGroup.VersionPatch = [int]$file.Project.PropertyGroup.VersionPatch + 1
        $file.Save("C:\Users\F12\source\repos\SharedList\SharedList\SharedListModels\SharedListModels.csproj")
        exit 1
    }
    
}

exit 0


