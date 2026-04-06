# build/download-assets.ps1
$Assets = Get-Content -Path "assets.json" | ConvertFrom-Json

$Assets.css | ForEach-Object {
    $OutFile = $_.dest
    $Dir = Split-Path $OutFile -Parent
    if (!(Test-Path $Dir)) { New-Item -ItemType Directory -Path $Dir -Force | Out-Null }
    Invoke-WebRequest -Uri $_.url -OutFile $OutFile
}

$Assets.js | ForEach-Object {
    $OutFile = $_.dest
    $Dir = Split-Path $OutFile -Parent
    if (!(Test-Path $Dir)) { New-Item -ItemType Directory -Path $Dir -Force | Out-Null }
    Invoke-WebRequest -Uri $_.url -OutFile $OutFile
}