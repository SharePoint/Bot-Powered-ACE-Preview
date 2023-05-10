# read-text-file

handles and strips byte order marks when reading a text file

```bash
npm install read-text-file --save
```

```javascript
var readTextFile = require('read-text-file');

var contentsPromise = readTextFile.read('path/to/file.txt');
var contents = readTextFile.readSync('path/to/file.txt');
```
