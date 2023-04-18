# HKSCM_PG3.3_Light
* **Author**: Debbie Wong debbie@ware.hk
* **Project link**: [PJ2208_HK Science Museum_PG Interactive 2021](https://drive.google.com/drive/folders/16270uT0EqpZD2BSOZLHnVnw37C7qTm9n)

## Project Description
HKSCM PG3.3 is a RPG game, standalone window program. It consists of 3 parts:
* [Main Game Program](https://github.com/WAREproject/PJ2208_HKSCM_PG3.3_RPG)
* Arduino Program for physical button
* Light Mini Program for physical lighting (current github project)
### Lighting Sequence
A summary of the lighting sequence is at this [excel table.](https://docs.google.com/spreadsheets/d/1EecS6_h7JKRmyrhct-eEEX2T0Yc3TJKX/edit?usp=sharing&ouid=114681440002746437125&rtpof=true&sd=true)
### System Structure
Main Game Program -> Light Mini Program -> Lighting Designer's Program for physical lighting (CueCore2)
* As CueCore2 can't perfrom loop command well, so a "Light Mini Program" is added to send out the cue command repeatedly in every 3~4 sec to realize the loop effect
* The "Main Game Program" will only send out situation command once when the target event is being triggered
* All command will be send out through UDP

## Development Environment
* Unity 2020.3.8
* Windows 11

## Resolution
* Landscape Windowed 1024x768

## Example
![image](https://user-images.githubusercontent.com/70264341/232674385-f9b35403-c4a6-447a-a548-c74e5ec50ec5.png)
```
{
   "ReceivedID":"301",
   "CueSteps":[
      {
         "SendID":"301_1",
         "Time":-1
      },
      {
         "SendID":"-1",
         "Time":0.5
      },
      {
         "SendID":"301_2",
         "Time":3
      }
   ]
}
```
Config json location: `/HKSCM_PG3.3_Light_Data/StreamingAssets/Config.json`

### Focus Area
Light Mini Program config is only focus on the column **"Situration, Light Cue# and Resend Time"** in the excel table. While the column "Duration and Lighting Status" is performed by the CueCore2.

### Situation 301
* Triggered when player discovered A boss. 
<br> The "Main Game Program" will send out `301`
<br> The "Light Mini Program" will receive by `"ReceivedID":"301"`
* There are 3 cue steps in the situation. So the "Light Mini Program" will have 3 `"CueSteps"` objects
* Cue Step 1: 
<br> **[301_1]** Use 0.5 sec to turn off all light, it will not resend.
<br> `"SendID":"301_1"` Represent the Light Cue#
<br> `"Time":-1` As it will not resend, so the time will be `-1`
* Cue Step 2: 
<br> Wait for 0.5 sec (wait till all light turn off and trigger the next step)
<br> `"SendID":"-1"`  For waiting time, use `"-1"`
<br> `"Time":0.5` The waiting time is 0.5 sec
* Cue Step 3: 
<br> **[301_2]** Use 1 sec to perform the breath effect for light A. As it is a loop event, it will resend the cue every 3 sec.
<br> `"SendID":"301_2"` Represent the Light Cue#
<br> `"Time":3` Resend every 3 sec
