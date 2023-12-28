=== Tutorial ===

= jump
{jump_explanation: ->jump_explanation}
-> jump_explanation

= jump_explanation
You can jump over this pit by pressing SPACE!
-> END

= jumpGamepad
You can jump over this pit by pressing the south button!
-> END

= crouching
That's a thin passage! 

But don't worry, I am sure you can fit through there with your new Form when pressing C!
->END

= crouchingGamepad
That's a thin passage! 

But don't worry, I am sure you can fit through there with your new Form when pressing the left shoulder!
->END

= firstBloom
I'm starting to feel a bit hungry, what about you?

Let's search for something to eat.
->END

= explosion
Oh no! What was that?

It sounded like it came from the direction your spaceship is lying.
->END

= sprinting
You can also hold SHIFT to sprint!

Might come in handy!
->END

= sprintingGamepad
You can also hold the west button to sprint!

Might come in handy!
->END

= stateChange
.

Oh I didn't expect that. What happened to you?

You look pretty sticky. Maybe you can take advantage from this to stick to a certain kind of surface.

Hopefully you can change back. Let's find out, press F!

->END

= stateChangeGamepad
.

Oh I didn't expect that. What happened to you?

You look pretty sticky. Maybe you can take advantage from this to stick to a certain kind of surface.

Hopefully you can change back. Let's find out, press the nord button!

->END

= theFlower
That's the kind of flowers that help us to grow up.

Maybe it will help you too, try taking the plant with E!
-> END

= theFire
Oh no, the forest is burning!

We must hurry!
-> END

= tipForFlower
I'm still hungry! Did you see the flower on the big branch?

It looked delicious, let's go and get it. 

-> END

= pokemonStory
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