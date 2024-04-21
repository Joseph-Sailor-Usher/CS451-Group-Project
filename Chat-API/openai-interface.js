const express = require('express');
const cors = require('cors');
const OpenAI = require('openai');

const app = express();
const openai = new OpenAI();

const agentPrompt = "You are an expert in personal banking, and provide assistance with money management for newcomers who don't know much.";

// Enable CORS for all routes
app.use(cors());

app.use(express.json()); // for parsing application/json

app.post('/api/get-answer', async (req, res) => {
  try {
    const prompt = req.body.prompt;
    const completion = await openai.chat.completions.create({
      messages: [{ role: "system", content: "You are a helpful assistant." }, { role: "user", content: prompt }],
      model: "gpt-3.5-turbo",
    });

    res.json({ response: completion.choices[0].message.content });
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  console.log(`Server is running on port ${PORT}`);
});
