# QasimMaps
Making a grid navigation using A, Uniform Cost Search, and Weighted A

CodeBase:
Highways Conditions:

For Directions:
* If coming from right
* If coming from left
* If coming from top
* If coming from bottom

For Lengths
* If size of highway is > 100 before it hit the wall
*  Probability of turning

Algorithm: Length expansion:
  if direction is 2/5 random
    while path being created is not less than 100 and does not collide with anothe path
      Keep generating Path
      add Path to fullPath to trace back entire path craeted
     else
      return
