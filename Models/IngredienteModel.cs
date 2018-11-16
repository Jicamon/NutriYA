using System;
using System.Collections.Generic;

namespace NutriYA{

    public class Ingrediente{
        public Ingrediente(){
            
        }

    public Ingrediente(string PK, string RK){
        Nombre = PK;
        OtraCosa = RK;
    }

    public string Nombre {get;  set;}
    public string OtraCosa {get; set;}

    }

}