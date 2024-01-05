const express = require('express');
const app = express();
const path = require('path')
const { v4 } = require('uuid')

const uniqueId = v4({});

app.use('/', express.static(path.join(__dirname, 'public')));
app.listen(3000, () => console.log('Listening on port 3000'));
app.get('/api/id', (_, res) => res.send(uniqueId))