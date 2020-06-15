<?php

    abstract class Parser
    {
        /**************************************************/
        /*Metodo che a seconda della sottoclasse che effettua il
         * suo override riceve una stringa e lo splitta secondo 
         * un qualche criterio definito nella sottoclasse*/
         /*************************************************/
        abstract public function SplitString($data);
        /**************************************************/
        /*Metodo che a seconda della sottoclasse che effettua il
         * suo override riceve un array di stringhe costruisce un oggetto
         * patendo da tale array secondo 
         * un qualche criterio definito nella sottoclasse*/
        /*************************************************/
        abstract public function BuildObject($splittedstring);
    }

?>