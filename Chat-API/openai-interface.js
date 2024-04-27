const express = require('express');
const cors = require('cors');
const OpenAI = require('openai');

const app = express();
const openai = new OpenAI();

const agentPrompt = "You are a bank assistant. Provide concise, relevant advice or information in under 72 characters. Focus on clarity and brevity.";

// Enable CORS for all routes
app.use(cors());

app.use(express.json()); // for parsing application/json

app.post('/api/get-answer', async (req, res) => {
  try {
    console.log('Received prompt:', req.body.prompt);
    const completion = await openai.chat.completions.create({
      messages: [{ role: "system", content: agentPrompt }, { role: "user", content: req.body.prompt }],
      model: "gpt-3.5-turbo",
    });

    console.log('OpenAI response:', completion);
    res.json({ response: completion.choices[0].message.content });
  } catch (error) {
    console.error('Error occurred:', error);
    res.status(500).json({ error: error.message });
  }
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  console.log(`Server is running on port ${PORT}`);
});
