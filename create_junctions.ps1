# Remove stale HSKMoreBalanceEvents junctions
$targets = @('C:\Program Files (x86)\Steam\steamapps\common\RimWorld\Mods\HSKMoreBalanceEvents')
$source = 'D:\Mods\HSKMoreBalanceEvents'
foreach ($t in $targets) {
    if (Test-Path $t) { cmd /c rmdir "$t" }
    cmd /c mklink /J "$t" "$source"
}
