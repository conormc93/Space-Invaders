### Developed By:
*Conor McGrath*

### Lecturer:
*Damian Costello*

## *Space Invader/Missile Command:*


## Running the program
This app was made using c#, Win2d and CanvasControl in VisualStudio __2017__

To clone the repository to your local machine, in command prompt enter 
```
git clone https://github.com/conormc93/Space-Invaders
```

#### Description
This is a UWP game using Win2d. The game incorporates an inclinometer that allows the user to tilt their device to control the pitch of the ship within the game. The game uses various game states to change the perspective of the screen. You can also save the high score and try and beat it when playing the game.

#### Research
As with any project, my first task was to break it down into manageable sections or pieces. I went online and found a few different resources on Win2d and Canvas Control. I had previously worked with Win2d outside of my course and I also had a little bit of experience with c# scripting, especially in games development from a PLC course I undertook.

For mapping the blasts from the ship to a path from where the user clicks, I researched other sources on linear interpolation. I was able to use the collision detection code I had from another module and engineer it into my game. 

I wanted to use multiple classes so that my code would not be in one big file so I looked up tutorials on how to call methods from other classes.

I have a class for storing your score and this was the trickiest part of the project for me because I had never really tried something like this before in c#. I got help for this from this article/website:[Accessing Files and Folders](http://www.domysee.com/blogposts/blogpost%209%20-%20changes%20to%20file%20reading%20in%20uwp%20apps/)

#### Design
For the design process, I took inspiration from arcade style games, mainly _Space Invaders_ and _Missile Command_. The images used in the game I apart from the three types of ships I created on Photoshop. I could of spent more time on the design aspect of the game but I spent a lot of time on the functionality of the game, and I ran into a lot of bugs in the development process.

#### What would I change
I will take this project on more when I have time to spare. Things I would like to add are:
- _Enemy health_
- _Player health_
- _Boss fight_
- _Enemies shoot back at you_

I would also like to fix the inclinometer. At the moment I only use the pitch but the roll and yaw methods could be implemented.
Also I would spend more time on developing the art of the game.

#### Conclusion
To summarize, I thoroughly enjoyed making this game. I learned a lot of new skills and would try and make another game in the future, or even build on this existing game.

#### Resources
- Damian Costello -- Galway-Mayo Institute of Technology
+ (https://stackoverflow.com/)
+ (http://www.domysee.com/blogposts/blogpost%209%20-%20changes%20to%20file%20reading%20in%20uwp%20apps/)
+ (https://msdn.microsoft.com/en-us/library/f02979c7.aspx)
+ (https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.canvas)
+ (https://github.com/Microsoft/Win2D)
+ (https://docs.microsoft.com/en-us/windows/uwp/gaming/e2e)
