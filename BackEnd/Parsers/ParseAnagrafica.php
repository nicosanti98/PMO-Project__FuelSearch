<?php
    require_once('Parsers/Parser.php');
    include 'ItemsDefinition/AnagraficaImpianto.php';
    
    class ParseAnagrafica extends Parser
    {
        //A partire da un array di stringhe creo una lista di oggetti di 
        //tipo anagrafica impianto
        public function BuildObject($parsedstring)
        {
            $list = [];
            $i; 
            for($i = 0; $i < count($parsedstring); $i++)
            {
                $item = new AnagraficaImpianto();
                
                $item->idImpianto = str_replace("'", " ", $parsedstring[$i]);
                $i++;
                $item->Gestore = str_replace("'", " ", $parsedstring[$i]);
                $i++;
                $item->Bandiera = str_replace("'", " ", $parsedstring[$i]);
                $i++;
                $item->TipoImpianto = str_replace("'", " ", $parsedstring[$i]);
                $i++;
                $item->NomeImpianto = str_replace("'", " ", $parsedstring[$i]);
                $i++;
                $item->Indirizzo = str_replace("'", " ", $parsedstring[$i]);
                $i++;
                $item->Comune = str_replace("'", " ", $parsedstring[$i]);
                $i++;
                $item->Provincia = str_replace("'", " ", $parsedstring[$i]);
                $i++;
                $item->Latitudine = str_replace("'", " ", $parsedstring[$i]);
                $i++;
                $item->Longitudine = str_replace("'", " ", $parsedstring[$i]);
                
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
            //nel file scaricato infatti i primi 11 elementi contengono
            //i nomi delle colonne
            $data = array_splice($data, 11);
            
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