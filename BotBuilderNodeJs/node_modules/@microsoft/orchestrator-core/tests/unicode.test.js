// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
const fs = require('fs');
const assert = require('assert');
const oc = require('../orchestrator-core.node');
const path = require('path');
require('fast-text-encoding'); 

// Finds onnx model
// Begins looking "path_depth" directories from current directory.
var findOnnxDir = function(path_depth, baseModelName='pretrained.20200924.microsoft.dte.00.06.en.onnx') {
  var cwd = process.cwd();
  var i;
  const sep = (cwd.includes('\\') ? '\\' : '/');
  for (i = 0; i< path_depth; i++)
  {
      var dtemp = cwd.split(sep);
      dtemp.pop();
      cwd = dtemp.join(sep);
  }

  var model_dir = null;
  function validateConfigFile(model_dir, baseModelName) {
      // console.log('ONNX: Found onnx file in ' + model_dir);
      try{
        var config_file = path.join(model_dir, 'config.json');
        // console.log('  => Seeing if ' + config_file + ' exists..')
        if (fs.existsSync(config_file)) {
            // console.log('      This file exists: ' + config_file);
            
            var jsonData = fs.readFileSync(config_file, 'utf8');
            const config = JSON.parse(jsonData); 
            if (config.hasOwnProperty('VocabFile')
                && config.hasOwnProperty('ModelFile') && config.hasOwnProperty('Name')
                && config.Name == baseModelName) {
                // console.log('     => VALID! Found model ' + baseModelName);
                return true;
            }
            // console.log('     => Invalid config!');
            return false;
        }
        // console.log('  => Does not exist: ' + config_file );
        return false;
      } catch(err) {
          // console.log('  => Does not exist EXCEPT!: ' + err );
          return false;
      }
      return false;
  }

  function findModel(cwd, baseModelName) {
     fs.readdirSync(cwd).find( (dirInner) => {
        dirInner = cwd + sep + dirInner;
        var stat = fs.statSync(dirInner);

        if (stat.isDirectory() && 
            !dirInner.endsWith("node_modules") &&  // Don't bother looking in node_modules
            !dirInner.endsWith("TestResults")) {  // Don't bother looking in TestResults
            return findModel(dirInner, baseModelName);
        } 

        if (stat.isFile() && dirInner.endsWith(".onnx")) {
            var dtemp = dirInner.split(sep);
            dtemp.pop();
            dirInner = dtemp.join(sep);
            if (validateConfigFile(dirInner, baseModelName)) {
              model_dir = dirInner;
              return true;
            }
            return false;
        }
        return false;
    });
    return model_dir;
  }

  findModel(cwd, baseModelName);
  return model_dir;
};

describe('Unicode', function () {

    // Set up a "test fixture" with loaded model.
    // Executed once for this overall suite.
    before(function () {
        this.timeout(1000000);
        this.orchestrator = new oc.Orchestrator();
        var onnx_dir = findOnnxDir(1);
        console.log(`DEBUGGING - nodejs - orchestrator-core: Orchestrator.before(): onnx_dir=${onnx_dir}`);
        fs.readdirSync(onnx_dir).forEach(file => {
            console.log(`DEBUGGING - nodejs - orchestrator-core: Orchestrator.before(): file=${file}`);
          });

        const load_result = this.orchestrator.load(onnx_dir);
        console.log(`DEBUGGING - nodejs - orchestrator-core: Orchestrator.before() DONE: load_result=${load_result}`);

        assert.strictEqual(load_result, true);

        console.log(`DEBUGGING - nodejs - orchestrator-core: Orchestrator.before() DONE: onnx_dir=${onnx_dir}`);
    });

    it('0) should add unicode text.', function () {
        // Arrange
        var labeler = this.orchestrator.createLabelResolver();
        const example = { 
            label: 'travel', 
            text: '我们刚才从.',
            };
        
        // Test
        const result = labeler.addExample(example);
        assert.strictEqual(result, true, 'Failed to add example');
    });
    it('1) should add unicode label name.', function () {
        // Arrange
        var labeler = this.orchestrator.createLabelResolver();
        const example = { 
            label: '图书馆来了', 
            text: 'Some crazy label name',
            };
        
        // Test
        const result = labeler.addExample(example);
        assert.strictEqual(result, true, 'Failed to add example');
    });

    it('2) should remove unicode label name.', function () {
        // Arrange
        var orc = new oc.Orchestrator();
        var onnx_dir = findOnnxDir(1);
        orc.load(onnx_dir);
        var labeler = orc.createLabelResolver();
        const example = { 
            label: '图书馆来了', 
            text: 'book a flight to miami.',
            };
        
        // Test
        var result = labeler.addExample(example);
        console.log(`DEBUGGING - nodejs - orchestrator-core: after calling labeler.addExample(), result=${result}, example=${example}`);
        assert.strictEqual(result, true, 'Failed to add example');
        const example2 = { 
            label: '图书馆来了', 
            text: 'book a flight to miami.',
            };
        result = labeler.removeExample(example2);
        console.log(`DEBUGGING - nodejs - orchestrator-core: after calling labeler.removeExample(), result=${result}, example2=${example2}`);
        assert.strictEqual(result, true, 'Failed to remove example');
    });
});

