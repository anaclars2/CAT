-> main

=== main ===
Mesmo você sendo de uma raça tão esquisita, a etiqueta amurita requer que eu me apresente direito. #speaker:Luna #portrait:luna-convencida
Eu sou Iaha-Mai, mas na sua língua, meu nome é Luna Leona, e eu venho do planeta Amura.  #speaker:Luna #portrait:luna-convencida
Eu diria que é um prazer te conhecer, mas na verdade está mais para um contratempo indesejado. #speaker:Luna #portrait:luna-normal
...
...
...
Você não vai se apresentar? #speaker:Luna #portrait:luna-raiva
    + [Apresentar-se à ela]
       Prazer, meu nome é Joe. #speaker:Joe #portrait:joe-fazendeiro-normal
       -> escolhaApresentar
    + [Ficar em silêncio]
    -> escolherNaoApresentar

== escolhaApresentar 
        Certo... #speaker:Luna #portrait:luna-normal
        -> DONE

== escolherNaoApresentar
        ... #speaker:Joe #portrait:joe-fazendeiro-normal
        Puff... #speaker:Luna #portrait:luna-raiva
        -> DONE
        