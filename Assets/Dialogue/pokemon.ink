INCLUDE globals.ink

{ pokemon_name == "": -> main | -> already_chose }

=== main ===
#audio:animal_crossing_low
Which pokemon do you choose?
    + [Charmander]
        -> chosen("Charmander")
    + [Bulbasaur]
        -> chosen("Bulbasaur")
    + [Squirtle]
        -> chosen("Squirtle")
        
=== chosen(pokemon) ===
~ pokemon_name = pokemon
You chose {pokemon}!
-> END

=== already_chose ===
You already chose {pokemon_name}!
-> END