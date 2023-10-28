# Pitch

> Pong is a great base mechanic. One of the most fundamental mechanic of a multiplayer game

# Features

## Power Ups

At the start of the game, each player will pick a power up, from then on, losing player get to pick a new upgrade

### Offensive

- Aim
  - Active
    Can redirect if the ball is still in player's area
    duration: once
    charge: 10s
  - Passive
    Aim on bounce back
- Invisible
  - Active
    on bounce back, ball will be invisible until it reaches enemy's area
  - Passive
    on bounce back, ball will be invisible for 0.3s
- Split
  - Active
    on bounce back, split the ball into 3 random direction, 2 of which will disappear when it reaches enemy's area
  - Passive
    on bounce back, split the ball into 2 random direction, 1 of which will disappear in 0.3s

### Defensive

- Reversal
  - Active
    reverse ball as soon as it enters player's area
    duration: once
    charge: 15s
  - Passive
    ball towards enemy is 10% faster
- Area where ball is slow towards player's goal
  - Active
    place a circle where the ball towards you will be 50% slower
    duration: 4s
    charge: 20s
  - Passive
    ball towards you is 10% slower
- Dash
  - Active
    dashes in a direction
    duration: once
    charge: 5s
  - Passive
    increase player speed 20%

### Special

- Move player's area
  Move player's area further to the enemy's
- Confusion
  Reverse enemy's control when the ball is in player's area
- bricks
  give bricks style defensive behind player

## Level Variety

### Different Shapes

### Dynamic Levels

- by use of shape keys
- by moving the camera

### Environment 'Power Ups'

## Lingo

- Player's Area
  The area between the dotted line and player's goal
