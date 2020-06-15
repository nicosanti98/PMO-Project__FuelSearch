<?php
    require_once('Parsers/Parser.php');
    include 'ItemsDefinition/Rilevazioni.php';
    
    class ParseRilevazioni extends Parser
    {
        //A partire da un array di stringhe creo una lista di oggetti di 
        //tipo rilevazioni
        public function BuildObject($parsedstring)
        {
            $list = [];
            $i; 
            for($i = 0; $i < count($parsedstring); $i++)
            {
                $item = new Rilevazioni();
                
                $item->idImpianto = str_replace("'", " ", $parsedstring[$i]);
                $i++;
                $item->descCarburante = str_replace("'", " ", $parsedstring[$i]);
                $i++;
                $item->prezzo = str_replace("'", " ", $parsedstring[$i]);
                $i++;
                $item->isSelf = str_replace("'", " ", $parsedstring[$i]);
                $i++;
                //Rimuove l'ora dalla data della rilevazione
                $parsedstring[$i] = preg_split("/ /", $parsedstring[$i])[0];
                $date = str_replace("'", " ", $parsedstring[$i]);
                $date = str_replace("/", "-", $parsedstring[$i]);
                $date = substr($date, 6, 4)."-".substr($date, 3, 2)."-".substr($date, 0, 2);
                $item->dtComu = $date;

                array_push($list, $item);
            }
            $this->list = $list;
        }
        
        public function SplitString($tosplit)
        {
            //Viene splittata la stringa passata in oggetto solamente quando
            //si incontrano caratteri di spaziatura o ; (carattere di separazione)ù
            //tipico del .csv
            $data = preg_split("/\n|;/", $tosplit);
            //Viene inserito in data l'array privato dei primi 11 elementi
            //nel file scaricato infatti i primi 6 elementi contengono
            //i nomi delle colonne
            $data = array_splice($data, 6);
            
            return $data;
        }
        
        //Se chiamato ritorna la lista di AnagraficaImpianto costruita
        //con i metodi precedenti
        public function TakeList()
        
        {
            return $this->list;
        }
        
        
        
        
    }
?>