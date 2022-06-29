# Vidyakali-Framework_CSharp
## Game Name 
Vidyakali (The knowledge to know the inner secret of the game in a challenging way).
## 	Short Description 
Vidyakali game is unique as it is represented by its name. There is a lot of thriller in this. The three unique levels that grapes the interest of the user. The first level starts with a single player and one chasing an enemy, there is a fight between them. If the enemy dies next level will be open. The second level has to fight with two chasing enemies, one is idle and the second one is running, the speed of running the enemy is greater than the idle enemy. Again if both enemies die a next-level box will open as the user capture it. In the third level, there is a lot of fun. The player can fire on the enemy and also fight with them as the enemy die the next enemy will be generated and so on. If the generating of the enemy reached its max point and all enemy die the player will win the game.
## 	Characters Description
Player: Vidyakali 

Enemy: 1. Idle 2. Running

Food: to boost the player's energy.

## 	Rules & Interactions
•	The player's health will decrease by 1 point after collision with an enemy
 
•	The enemy's health will decrease by 1 point after fighting with the player.

•	As the player's health decrease to zero next lifeline will use

•	As the player gets energy points it will increase the player's lifeline by 20%.

•	As the health of the enemy is zero the enemy will die and the new enemy will generate depending upon level number.

•	In third level you cannot boost health game will puse.
## 	Goal of the Game
The main objective behind this game is very simple. The user can utilize their time while playing this game and don't spend time in the bad company of society. The vidyakali and enemy help the player to fight with the external enemy of the society in a good way and take power from it. This three-level is the spirit of our religion stop the evil with the hand, tongue, and dislike it in the heart.
## Features of Framework
Following are the features of the framework.
 •	Movement
 
     o	Keyboard
     
     o	Horizontal
     
     o	Chasing
     
•	Firing

    Proper firing system of the player towards the enemy.

•	 Collision

    o	Energy Point

    o	Enemy 

    o	Player


•	Health System

    o	Player

    o	Enemy

•	Score

    Increase the game score according to the set value of the user.


• Extendibility

    Ability of extent all these according to the new user’s requirement.

## Class diagram

![Movement](https://user-images.githubusercontent.com/96945594/175982386-ab0882f6-5f88-4c3e-a701-b761c1725b23.jpg)
![Score](https://user-images.githubusercontent.com/96945594/175982378-f8c0c344-d0fd-4718-9cbe-b8d8d12bb6bb.jpg)
![Core](https://user-images.githubusercontent.com/96945594/175982511-cafbf0d5-e492-4810-acfb-cf9f13f70cf1.jpg)
![Collsion](https://user-images.githubusercontent.com/96945594/175982581-797fd9c4-961c-4e31-9689-6b4fcef1e211.jpg)

## 	Framework Implementation
The Vidyakali framework is easy to implement
    You need to add the Dynamic-link library(DLL) file in your C# Window form references.

    o	Right-click on the references

    o	Left-click on the add references

    o	In the assembblies>Framework>Framework select the  file
Initialization of the game using following parameterize 

    Game(int gravity,int reducePlayerHealth,int reduceEnemyHealth,float scoreIncrementValue,bool gameStatus)
