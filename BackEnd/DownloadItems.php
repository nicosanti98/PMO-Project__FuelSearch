<?php

    class DownloadItems
    {
        private $url; 
        
        //Costruttore che riceve l'url dal quale scaricare
        //e lo memorizza nell'attributo url
        public function __construct($url)
        {
            $this->url = $url;
        }
        
        //Tramite questo metodo è possibile scaricare una stringa
        //da un file su un URL
        public function GetDownloadedString()
        {
            $string = file_get_contents($this->url);
            return $string;
        }
    }
    
?>