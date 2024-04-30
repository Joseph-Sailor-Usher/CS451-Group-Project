document.getElementById('chatForm').addEventListener('submit', function (event) {
     event.preventDefault();
     const prompt = document.getElementById('prompt').value;


     // Validate that the prompt is within [10 - 100] characters, contains no swear words, and is relevant to banking
     if (prompt.length < 10) {
          document.getElementById('response').innerText = 'Prompt must be at least 10 characters long';
          return;
     }
     if (prompt.length > 100) {
          document.getElementById('response').innerText = 'Prompt must be at most 100 characters long';
          return;
     }

     let badWords = [
          "anal",
          "anus",
          "arse",
          "ass",
          "ballsack",
          "balls",
          "bastard",
          "bitch",
          "biatch",
          "bloody",
          "blowjob",
          "blow job",
          "bollock",
          "bollok",
          "boner",
          "boob",
          "bugger",
          "bum",
          "butt",
          "buttplug",
          "clitoris",
          "cock",
          "coon",
          "crap",
          "cunt",
          "damn",
          "dick",
          "dildo",
          "dyke",
          "fag",
          "feck",
          "fellate",
          "fellatio",
          "felching",
          "fuck",
          "f u c k",
          "fudgepacker",
          "fudge packer",
          "flange",
          "Goddamn",
          "God damn",
          "hell",
          "homo",
          "jerk",
          "jizz",
          "knobend",
          "knob end",
          "labia",
          "lmao",
          "lmfao",
          "muff",
          "nigger",
          "nigga",
          "omg",
          "penis",
          "piss",
          "poop",
          "prick",
          "pube",
          "pussy",
          "queer",
          "scrotum",
          "sex",
          "shit",
          "s hit",
          "sh1t",
          "slut",
          "smegma",
          "spunk",
          "tit",
          "tosser",
          "turd",
          "twat",
          "vagina",
          "wank",
          "whore",
          "wtf"
     ];
     for (let word of badWords) {
          if (prompt.includes(word)) {
               document.getElementById('response').innerText = 'Prompt contains inappropriate language';
               return;
          }
     }



     // Append user message to chatbox
     var userDiv = document.createElement("div");
     userDiv.className = "message user";
     userDiv.innerHTML = "<strong>You:</strong> " + prompt;
     document.getElementById('chatbox').appendChild(userDiv);

     fetch('http://localhost:3000/api/get-answer', {
          method: 'POST',
          headers: {
               'Content-Type': 'application/json'
          },
          body: JSON.stringify({ prompt: prompt })
     })
          .then(response => response.json())
          .then(data => {
               // Append bot response to chatbox
               var botDiv = document.createElement("div");
               botDiv.className = "message bot";
               botDiv.innerHTML = "<strong>Timmy:</strong> " + data.response;
               document.getElementById('chatbox').appendChild(botDiv);

               // Scroll to the bottom of chatbox
               document.getElementById('chatbox').scrollTop = document.getElementById('chatbox').scrollHeight;
          })
          .catch(error => {
               console.error('Error:', error);
          });

     // Clear input field
     document.getElementById('prompt').value = "";
});
// JavaScript to prevent dropdown menu from closing on clicking inside
document.getElementById('chatToggle').addEventListener('click', function (event) {
     event.stopPropagation();
});

// JavaScript to stop click event propagation for the input box
document.getElementById('prompt').addEventListener('click', function (event) {
     event.stopPropagation();
});
