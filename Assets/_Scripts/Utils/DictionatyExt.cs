using System.Collections.Generic;
using System.Linq;
using System;

public static class DictionatyExt
{
    public static IEnumerable<TValue> RandomValues<TKey, TValue>(this IDictionary<TKey, TValue> target) 
    { 
        Random rand = new Random(); 

        List<TValue> values = Enumerable.ToList(target.Values);
        
        int size = target.Count; while (true) 
        {
            yield return values[rand.Next(size)]; 
        } 
    }

    // Fuente: https://www.iteramos.com/pregunta/61923/entrada-al-azar-del-diccionario
}
