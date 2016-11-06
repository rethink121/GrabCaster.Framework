Param($DataContext)

        $Dir = get-childitem "C:\Program Files (x86)\GrabCaster\Demo\File2File\PowershellIn" -recurse
        # $Dir |get-member
        $files = $Dir | where {$_.extension -eq ".txt"}
        for ($i=0; $i -lt $files.Count; $i++) {
            $outfile = $files[$i].FullName + ".done" 
            $DataContext = Get-Content $files[$i].FullName
            rename-item -path $files[$i].FullName -newname $outfile

        }
