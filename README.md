# Normal-Distribution-Wheel

![Unity Version](https://img.shields.io/badge/Unity-2021.3%2B-blue.svg)
![License](https://img.shields.io/badge/License-MIT-green.svg)

## Overview

**Normal-Distribution-Wheel** is an interactive "Wheel of Fortune" game developed in Unity. This project uniquely integrates the statistical **68–95–99.7 rule** of the normal distribution, utilizing the **Box-Muller method** to determine the probability of each slice. The game offers players a chance to spin the wheel, with outcomes influenced by a Gaussian distribution, providing an educational and entertaining experience.

## Features

- **Probability-Based Outcomes**: Slices are weighted according to the normal distribution:
  - **Within 1σ**: Empty slices.
  - **Between 1σ and 2σ**: Grant an extra spin.
  - **Between 2σ and 3σ**: Award a prize.
- **Gameplay Mechanics**:
  - The wheel comprises **12 slices**, optimized for this distribution model.
  - Each player is allotted **10 spins** per session.
  - The game concludes when a player either exhausts all spins or wins the prize.
- **Data Tracking**: At the end of each session, results are saved to a file, calculating the percentage of players who successfully won the prize.

## Installation

1. **Clone the Repository**:
- git clone https://github.com/mryusefi/Normal-Distribution-Wheel.git
2. **Open in Unity**:
- Launch Unity Hub.
- Click on the **"Add"** button and select the cloned project directory.
- Open the project in Unity Editor.

## How to Play

1. **Start the Game**: Press the **"Play"** button in the Unity Editor or build the project for your preferred platform.
2. **Spin the Wheel**: Click on the **"Spin"** button to rotate the wheel.
3. **Outcome Determination**:
- If the wheel lands on an empty slice, no reward is given.
- Landing on a slice between 1σ and 2σ grants an extra spin.
- Landing between 2σ and 3σ awards a prize, ending the game.
4. **Game End**: The session concludes when all spins are used or the prize is won.
5. **Result Logging**: After each session, results are logged, and the success rate is updated.

## Technical Details

- **Normal Distribution Implementation**: The game employs the **Box-Muller transform** to generate normally distributed random numbers, aligning slice probabilities with the 68–95–99.7 rule.

## Contributing

Contributions are welcome! If you have suggestions for improvements or new features, please fork the repository and submit a pull request.

---

Enjoy spinning the wheel and may the odds be in your favor!
