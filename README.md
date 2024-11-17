# Ghost Blaster 🎮👻

**Ghost Blaster** is an intense 3D endless shooter game developed by a team of three passionate developers at the University of Southampton Malaysia. This game challenges players to survive and score as high as possible within a one-minute time limit. As the game progresses, enemy numbers and spawn rates scale up, creating a continuous challenge that demands quick reflexes, precise aim, and smart movement.  

## 🎯 Objective

Achieve the highest possible score within the one-minute time limit while dodging enemy attacks and strategically defeating waves of enemies.  

---

## 🚀 Key Features

### Core Game Mechanics
1. **Dynamic Enemy AI**  
   - Enemies patrol, chase, and attack based on player proximity.  
   - Enemy projectiles are directed accurately toward the player using Object Pooling for efficiency.  

2. **Scaling Difficulty**  
   - Spawn rates and enemy counts increase as the player scores higher, adding escalating difficulty to keep the challenge fresh.  

3. **Player Movement**  
   - Dodge and reposition to outmaneuver enemies, enhancing survivability as difficulty rises.  

4. **Time-Limited Challenge**  
   - Players have one minute to achieve the highest score possible, pushing them to act quickly and strategically.  

---

## 🛠 Technical Details

### Engine  
- Unity, with 3D environments and NavMesh for enemy pathfinding.  

### AI Mechanics  
- Enemies use patrol, chase, and attack states based on player distance, supported by NavMesh.  
- Projectiles directed accurately using Object Pooling for enhanced performance.  

### Animations and Effects  
- Includes running, attacking, dissolving animations.  
- Audio effects include gun sounds, muzzle flashes, and impact effects for an immersive experience.  

### Difficulty Scaling  
- A score-based system adjusts spawn rates and enemy numbers as the player progresses, keeping the gameplay dynamic.  

### Scripting and Events  
- C# scripting powers AI behaviors, movement, scoring events, and a custom event system for enemy death notification to manage game flow.  

---

## 🎮 Gameplay

### Tutorial Menu  
- **Play With Tutorial**: Learn the game mechanics before playing.  
- **Without Tutorial**: Jump straight into the game if you're ready.  

### Main Menu  
- Start your adventure or explore the tutorial mode to familiarize yourself with the controls and mechanics.  

---

## 🏆 Challenges and Innovations  

1. **Dynamic Enemy AI**  
   - Creating a fluid, realistic AI behavior for patrol, chase, and attack while ensuring smooth transitions using NavMesh and custom scripts.  

2. **Efficient Projectile Management**  
   - Optimized performance with Object Pooling to handle large numbers of projectiles.  

3. **Difficulty Scaling by Score**  
   - Implemented score-based scaling for progressive gameplay difficulty.  

4. **Enhanced Visual Feedback**  
   - Animations for enemy states (running, attacking, dissolving) and visual/audio effects provide players with clear feedback on their actions.  

---

## 🖥 Team Credits  

| Role                | Name          |  
|---------------------|---------------|  
| **Enemy Mechanic**  | Prabhjot Singh |  
| **Player Mechanic** | Zong Ze        |  
| **Map Design**      | Jeremiah Lee   |  

---

## 📸 Screenshots  

### Main Menu  
![Main Menu](Images/Main%20Menu.png)  

### Tutorial Menu  
![Tutorial Menu](Images/Tutorial%20Menu.png)  

### Enemy Design  
![Enemy Design](Images/Enemy%20Design.png)  

### Player Design  
![Player Design](Images/Player%20Design.png)  

---

## 📥 Installation  

1. **Clone the repository**:  
   ```bash  
   git clone https://github.com/your-repo/ghost-blaster.git  
   ```  

2. **Open in Unity**:  
   - Make sure you have Unity installed.  
   - Open the project in the Unity Editor.  

3. **Build and Play**:  
   - Configure the build settings for your platform.  
   - Run the game!  

---

## 🔧 Future Improvements  

- Implementing online leaderboards for global competition.  
- Expanding the map with interactive environments.  
- Adding power-ups and new enemy types for diversified gameplay.  
