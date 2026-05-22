# Goal

The goal of this epic is to prepare the project for the upcoming release of Forza Horizon 6. Right now, the site is only set up to function with FH5 by default, and there's not an easy way to add new courses without directly adding to the database, nor is there any infrastructure set up to support multiple games.

# Basic Requirements

- Courses need to be stored in the database so more can be added through the api and dynamically loaded on the frontend
- The user will need to be able to switch between more than one game (just Forza Horizon 5 & 6 for now) and select from that game's races
- Any other features the AI deems useful will be considered
- Any suggestions the AI has for the current analysis information/display will be considered

# Assumptions
- All the courses/events currently on the site belong to Forza Horizon 5