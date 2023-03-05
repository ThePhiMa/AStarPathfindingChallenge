# AStarPathfindingChallenge
A quick coding challenge I did on for an interview at a well-respected video game studio in February 2023.
The task was to create a small Unity project that implements pathfinding without using the in-build navmesh.

Total hours spend: 11
- 1.5h for research (state of the art for pathfinding algorithms, different optimization techniques like D*, etc.)
- 2h for setting up the code structure and basic 2d grid generation
- 2.5h for A* coding
- 1h for refactoring the code structure
- And way too long for playing around with different smoothing algorithms for the pathfinding. I really wanted to use bézier smoothing and tried implementing some ideas, but I eventually reached the time limit of 10 hours I set for myself and then had to quickly hack together some poorly working diagonally path smoothing.
- 1 last hour to clean up the code

Final thoughts on the code:
- I really wanted to use generics because I like using them since they are the embodiment of the DRY principle (and it feels fancy), but I don’t care much that now the AStarPathfinding class needs to know the exact type of GridNode2D. But finding a solution for this felt out of scope.
- I didn't really use weights for the A*, e.g. for roads to be "quicker" than normal terrain, since I really wanted for the map to be randomly generated on start and complex procedural generation (e.g. for roads) seemed out of scope too.

Assets:
1-bit Pack by Kenney: https://www.kenney.nl/assets/bit-pack

Made in Unity 2022.2.2f1
