<?php

	$fileName = $_POST["fileName"];
	$folderName = $_POST["folderName"];
	$ledNumber = $_POST["ledNumber"];
	$reactionTime = $_POST["reactionTime"];
	$reactionType = $_POST["reactionType"];
	
	
	if(($file = fopen("../files/".$folderName."/".$fileName.".csv", "a")) !==FALSE){
		if(flock($file,LOCK_EX)){
			fputcsv($file, array($ledNumber,$reactionTime,$reactionType));
		}else{
			echo "Error: Please Reload Page";
		}			
		fclose($file);
	}
?>