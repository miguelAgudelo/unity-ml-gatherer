# Unity machine learning project for object collector agent
This first ml project in unity sought to achieve that this agent, which simulates a drone or other object that can float and move in various directions (left / right / up / down / front), and rotate on its own axis, is Able to collect different objects referred to in the project as packages and take them to a defined position, through obstacles on a first floor and a clear second floor, through which it can access through several holes. Here are some aspects to highlight the operation.

![alt text](https://github.com/miguelAgudelo/unity-ml-gatherer/blob/main/images/gathererML.gif)

## Installation
Please refer to the Unity ML Agents repository, Version 1.0.6

## Agent:

### Inputs:

Raycast: The agent contains 3 downward-facing and 3 upward-facing raycast sensing 3D sensors and one for the front, which has multiple beams to give you a wide viewing area.

![alt text](https://github.com/miguelAgudelo/unity-ml-gatherer/blob/main/images/gathererMl1.png)

Observations:

* it is observed if the collector owns a package or not (1 bool = 1 value)
* distance to the designated object for package delivery (1 float = 1 value)
* address to the object designated for package delivery 1 Vector3 = 3 values)
* number of packages remaining (1 int = 1 value)
![alt text](https://github.com/miguelAgudelo/unity-ml-gatherer/blob/main/images/gathererMl2.png)
Vector observation = 6

### Outputs

space type = Discrete

* can rotate on its own axis
* can move left or right
* can move up or down
* can advance

## The Rewards

Contact with the package (object to be collected) :The agent has a small object in the lower part which when colliding with the package gives him a reward, and this package adheres to it, in the same way when the package is attached to the agent it is when it collides with the designated area (platform ), it disappears and the agent receives a reward. If all packages are delivered, you get extra reward.

![alt text](https://github.com/miguelAgudelo/unity-ml-gatherer/blob/main/images/gathererMl3.png)

## The penalty:

Punishment for time to force the network to seek rewards, and punishment for colliding with certain objects, to limit the agent's learning area.

## Curriculum

For the correct training of the agent, the curriculum learning was used, with 4 lessons

1. lesson one: Only 15 packs with only first floor active, invisible roof limits agent movement area

![alt text](https://github.com/miguelAgudelo/unity-ml-gatherer/blob/main/images/gathererMl4.png)

2. Lesson two: Only 15 packs on the first floor and second floor is activated with 15 packs, invisible floor is disabled

![alt text](https://github.com/miguelAgudelo/unity-ml-gatherer/blob/main/images/gathererMl5.png)

3. Lesson three: with 20 packs on the first floor, obstacle-based walls are added

![alt text](https://github.com/miguelAgudelo/unity-ml-gatherer/blob/main/images/gathererMl6.png)

4. Lesson four: With 25 packages on the first floor with walls and second floor with wall in the middle to force the agent to go down to the first floor at the farthest part of the platform

![alt text](https://github.com/miguelAgudelo/unity-ml-gatherer/blob/main/images/gathererMl7.png)

5. Lesson five: With 25 packages on the first floor with walls and second floor with 15 packages and without walls

![alt text](https://github.com/miguelAgudelo/unity-ml-gatherer/blob/main/images/gathererMl8.png)

## Configuration file .yaml

important to highlight the use of memory and curiosity for the operation of the project

![alt text](https://github.com/miguelAgudelo/unity-ml-gatherer/blob/main/images/trainer_config_curriculum_g.yaml.png)






