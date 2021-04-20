$myObject = [PSObject]@{
    Name     = 'Kevin'
    Language = 'PowerShell'
    State    = 'Texas'
}

$myObject.GetType().GetMember('Properties', 'Public, NonPublic, FlattenHierarchy, Instance') | ft *