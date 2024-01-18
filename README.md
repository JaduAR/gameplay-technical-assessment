Instructions
1. Download Unity 2021.3 and clone this repository
2. Install git lfs and pull lfs files (without doing a git lfs pull, you will be missing assets)
3. Implement a simple fighting game that achieves the gameplay described below
4. Detailed instructions and a quick overview of provided repository can be found in the video [here](https://drive.google.com/file/d/1P1-U5Nxe4aGmFnl10HNCQeGL-iMivAWg/view?usp=sharing)
5. Sign the technical test waiver located in Assets/Signed Waiver/ folder and save as [Your Name] - Signed Waiver
6. Submit a PR titled [Your Name] Simple Fight
7. Sit tight! We will get back to you shortly

Gameplay Overview:
- Player uses WASD / arrow to control avatar movement
- Player attacks opponent using the attack button
- Opponent is a bot that moves around on its own and attempt to avoid player attacks
- The game ends when the player has either depleted thier opponent's health by landing Punch 1 (P1) and/or Punch 2 (P2) attacks. Or landing a Heavy Punch will knock out opponent instantly.
- Note: the player should always win as you do not need to program the bot to attack back 

Attack Sequence:
- On tap, transition into a Punch 1 (P1) or Punch 2 (P2)
- If another tap is executed within 500 ms, transition from P1 to P2 or P2 to P1
- If both P1 and P2 land the next button press should transition into a heavy punch charge
- When button is released, transition from charge to heavy punch
- If P1 and/or P2 do not land, we transition back to idle and the next tap will be either P1 or P2 based on the last executed move

Health and Damage:
- Opponent starts game with 100 health points
- P1 and P2 attacks deal 10 damage points when they land
- Heavy punch instantly knocks out opponent, bringing their health to 0 and ending the game

Provided Assets:
- Starting scene
- Player and opponent avatars
- Base movement animations
- Attack animations
- Animator Controller to build off of
- Attack button icon

[OPTIONAL] Bonus / Polish Suggestions:
- Lock each avatar to always be facing opponent (this is something we do in our game so attacks land easier)
- Implement a healthbar UI to indicate opponent’s health
- Add visual and/or sound effects to indicate when an attack has landed (utilizing free assets from the asset store is perfect for this)
- Whatever else you want to do to show off your unique skillset as a game developer!



  
