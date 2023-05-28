Do you like anime? # speaker: Bob
*Yes, [I do!] thanks for asking! # speaker: Karen
Me too!
*No 
Oh, too bad.
- -> main

=== main ===
Which pokemon do you choose?
+[Charmander]
-> chosen("Charmander")
*[Bulbasaur]
-> chosen("Bulbasaur")
*[Squirtle]
-> chosen("Squirtle")

=== chosen(pokemon) ===
You chose {pokemon}!
-> END