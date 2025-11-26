<div align="center">

<h1>⚔️ To The Reach</h1>
<h3>Narrative-Driven 2D Action Platformer</h3>

 <a href="https://omerburakozgur.itch.io/to-the-reach" target="_blank">
    <img src="https://img.shields.io/badge/Play_on-Itch.io-FA5C5C?style=for-the-badge&logo=itch.io&logoColor=white" alt="Play on Itch.io">
  </a>
 <a href="https://docs.google.com/spreadsheets/d/170ER9QGe05l_hQTVN4tuNwHNAqZiFPzERh6UPb0jh7o/edit?usp=sharing" target="_blank"> <img src="https://img.shields.io/badge/Document-2B5797?style=for-the-badge&logo=googledocs&logoColor=white" alt="Document"></a>
   <a href="https://youtu.be/hN8WNtm1iks" target="_blank"> <img src="https://img.shields.io/badge/Watch-Tech_Demo-FF0000?style=for-the-badge&logo=youtube&logoColor=white" alt="Watch Video"> </a> 
   <a target="_blank" href="https://www.artstation.com/artwork/GvldvB"> <img alt="ArtStation" src="https://img.shields.io/badge/Visuals-Artstation-13AFF0?style=for-the-badge&logo=artstation&logoColor=white"> </a> </p>

<h3>🎥 Gameplay Showcase (Click to Watch)</h3>
<a href="https://youtu.be/hN8WNtm1iks" target="_blank" title="Click to watch full gameplay video">
  <img src="https://github.com/user-attachments/assets/53633927-a7a6-446c-8907-cebc9f14482e" alt="To The Reach Gameplay GIF" width="800" style="max-width:100%; border-radius: 8px; border: 2px solid #FF0000;">
</a>

<br><br>

| **Engine** | **Language** | **Art Tool** | **Input System** |
| :---: | :---: | :---: | :---: |
| <img src="https://img.shields.io/badge/Unity_2D-000000?style=flat-square&logo=unity&logoColor=white" /> | <img src="https://img.shields.io/badge/C%23-239120?style=flat-square&logo=c-sharp&logoColor=white" /> | <img src="https://img.shields.io/badge/Aseprite-7D55EC?style=flat-square&logo=aseprite&logoColor=white" /> | <img src="https://img.shields.io/badge/New_Input_System-white?style=flat-square&logo=unity&logoColor=black" /> |

</div>

---

## 📖 Project Overview

**To The Reach** is a 2D side-scroller action-adventure game developed by me over the course of 3-4 months. The project showcases a complete development pipeline, from implementing complex mechanics to managing game states.

The game features **5 playable characters** with unique skill sets, engaging combat mechanics (combos, air attacks, dodging), and a narrative-driven progression system culminating in intense boss battles.

---

## 🎮 Key Features

* **Complex Combat System:** Implemented attack combos, air attacks, special abilities, and a sliding/dodge mechanic to create fluid gameplay.
* **Diverse Character Roster:** Programmed logic for 5 different characters, each requiring unique animation trees and state handling.
* **Boss Battles:** Scripted logic for **2 Mini-Bosses** and **1 Main Boss**, featuring distinct attack patterns and phases.
* **Platforming Mechanics:** Integrated classic elements such as double jumps, ladders, traps, and parkour challenges.
* **Responsive UI:** Designed a responsive User Interface including Health Bars, Menus, and Dialogue systems adapting to screen ratios.

---

## ⚙️ Technical Deep Dive

This project was a major milestone in understanding scalable game architecture. I moved beyond simple scripts to create modular systems.

### 🧠 Finite State Machines (FSM)
Instead of complex boolean flags, I utilized **Unity Animator** as a visual State Machine to handle character states effectively.
* **Implementation:** States such as `Idle`, `Run`, `Attack1`, `Attack2`, `Dodge`, and `Hurt` are managed via Animation Parameters controlled by C# scripts.
* **Benefit:** This ensured smooth transitions between animations and prevented logic bugs (e.g., taking damage while dodging).

### 🏗️ Manager Architecture
To decouple systems, I implemented a centralized Manager structure:
* **Game Manager:** Handles global game state (Pause, Game Over, Victory).
* **Audio Manager:** Manages BGM and SFX pooling.
* **Quest & NPC Managers:** Handles dialogue states and objective tracking.

---

## 🎨 Art & Asset Pipeline

As a solo developer, I **prioritized gameplay programming and system architecture**. To achieve a polished look within a limited timeframe, I utilized a hybrid workflow of curated free assets and custom modifications.

* **Asset Sourcing:** Curated and integrated high-quality free assets from platforms like **Itch.io**, **Unity Asset Store**, and **Craftpix** to build the visual foundation.
* **Modification & Polish:** Used **Aseprite** to edit, recolor, and modify existing sprites/tilesets to ensure a cohesive art style across different asset packs.
* **Integration:** Focused on seamlessly blending these disparate assets into the Unity environment using lighting (URP 2D Lights) and particle effects to create a unified atmosphere.

---

## 📥 Installation

You can download the playable build via **Itch.io** or directly from **GitHub Releases**.

### Option 1: Itch.io (Recommended)
1.  Go to the [Itch.io Page](https://omerburakozgur.itch.io/to-the-reach).
2.  Click the **Download** button for the latest Windows build.
3.  Extract the `.zip` file to a folder.
4.  Run `ToTheReach.exe`.

### Option 2: GitHub Releases
1.  Click on the **[Releases](../../releases)** section on the right sidebar of this repository.
2.  Find the latest version tag (e.g., `v1.0`).
3.  Under "Assets", download the `ToTheReach_Build.zip` file.
4.  Extract and run `ToTheReach.exe`.

*Enjoy the adventure!*

---

<div align="center">
  <a href="mailto:omerburakozgur1@gmail.com">
    <img src="https://img.shields.io/badge/Contact_Me-D14836?style=for-the-badge&logo=gmail&logoColor=white" alt="Email">
  </a>
  <a href="https://www.linkedin.com/in/omerburakozgur/" target="_blank">
    <img src="https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white" alt="LinkedIn">
  </a>
</div>
