<?php
	//Funktionen
	//Überprüft ob der Wert im Array ist
	function IsValueInArray($wert,$array)
	{
		$ret=false;
		for ($i=0;$i < count($array);$i++)
		{
			 if ($wert==$array[$i])
			 {
			    $ret=true;    
			 }
		}
		return $ret;
	}
	
	//Konstanten
	//Die erlaubten Spiele
	$allowedGames = array("winemaker", "ohtest");
	$maxEntries = 10;

	//Variablen
	//if (isset($_POST['game'])) $name = $_POST['game'];
  //if (isset($_POST['gameid'])) $name = $_POST['gameid'];
	//if (isset($_POST['name'])) $name = $_POST['name']; 
	//if (isset($_POST['points'])) $name = $_POST['points']; 
	//if (isset($_POST['fv1'])) $name = $_POST['fv1']; 
	//if (isset($_POST['fv2'])) $name = $_POST['fv2']; 
	//if (isset($_POST['fv3'])) $name = $_POST['fv3']; 
	//if (isset($_POST['fv4'])) $name = $_POST['fv4']; 
	//if (isset($_POST['fv5'])) $name = $_POST['fv5']; 
	//if (isset($_POST['chksum'])) $name = $_POST['chksum']; 
	
	//Debugvarss
	$game="ohtest";
  $gameid="2008202284-6545698665-655985";
	$name="Florian Bottke";
	$points="123456";
	$fv1="3";
	$fv2="2";
	$fv3="1";
	$fv4="4";
	$fv5="0";
	$chksum="00aa00aa00";

	echo "sum";

	//Check ob das Spiel zu den erlaubten Spielen gehört
	$ValueInArray = IsValueInArray($game, $allowedGames);
	if($ValueInArray==true) //Spiel ist in Erlaubter Liste
	{
		//Daten mit Checksumme vergleichen
		
		//Datei öffnen
		$filename = "$game.txt";
		$datafile = fopen($filename, "w");
		fwrite($datafile,"Daten"); //Debug
		fclose($datafile);

		//Highscore einlesen
		$HighscoreArray = file($filename);

		//Idee für Einfügen
		//Array durchgehen
		//Eintrag merken ab dem das welcher niedgriger ist
		//beim neuschreiben dieses eintrag mit reinschreiben
		//alles länger als max entries fliegt raus
		
		//Schauen ob der Eintrag Highscorewürdig ist und eintragen
		
		//Highscore echo

		//Datei schließen
		

		//Debug
		readfile($filename);
		echo $ValueInArray;
		//echo $game;
		//echo $game;
		echo $filename;
	}
?>
