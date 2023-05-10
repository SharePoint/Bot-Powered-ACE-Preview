/**
 * call-matcher:
 *   ECMAScript CallExpression matcher made from function/method signature
 *
 * https://github.com/twada/call-matcher
 *
 * Copyright (c) 2015-2019 Takuto Wada
 * Licensed under the MIT license.
 *   https://github.com/twada/call-matcher/blob/master/MIT-LICENSE.txt
 */
'use strict';

const estraverse = require('estraverse');
const espurify = require('espurify');
const syntax = estraverse.Syntax;
const hasOwn = Object.prototype.hasOwnProperty;
const deepEqual = require('deep-equal');
const notCallExprMessage = 'Argument should be in the form of CallExpression';
const duplicatedArgMessage = 'Duplicate argument name: ';
const invalidFormMessage = 'Argument should be in the form of `name` or `[name]`';

class CallMatcher {
  constructor (signatureAst, options) {
    validateApiExpression(signatureAst);
    options = options || {};
    this.visitorKeys = options.visitorKeys || estraverse.VisitorKeys;
    if (options.astWhiteList) {
      this.purifyAst = espurify.cloneWithWhitelist(options.astWhiteList);
    } else {
      this.purifyAst = espurify;
    }
    this.signatureAst = signatureAst;
    this.signatureCalleeDepth = astDepth(signatureAst.callee, this.visitorKeys);
    this.purifiedCallee = this.purifyAst(this.signatureAst.callee);
    this.numMaxArgs = this.signatureAst.arguments.length;
    this.numMinArgs = this.signatureAst.arguments.filter(isIdentifier).length;
  }
  test (currentNode) {
    if (this.isCalleeMatched(currentNode)) {
      const numArgs = currentNode.arguments.length;
      return this.numMinArgs <= numArgs && numArgs <= this.numMaxArgs;
    }
    return false;
  }
  matchArgument (currentNode, parentNode) {
    if (isCalleeOfParent(currentNode, parentNode)) {
      return null;
    }
    if (this.test(parentNode)) {
      const indexOfCurrentArg = parentNode.arguments.indexOf(currentNode);
      let numOptional = parentNode.arguments.length - this.numMinArgs;
      const matchedSignatures = this.argumentSignatures().reduce((accum, argSig) => {
        if (argSig.kind === 'mandatory') {
          accum.push(argSig);
        }
        if (argSig.kind === 'optional' && numOptional > 0) {
          numOptional -= 1;
          accum.push(argSig);
        }
        return accum;
      }, []);
      return matchedSignatures[indexOfCurrentArg];
    }
    return null;
  }
  calleeAst () {
    return this.purifiedCallee;
  }
  argumentSignatures () {
    return this.signatureAst.arguments.map(toArgumentSignature);
  }
  isCalleeMatched (node) {
    if (!isCallExpression(node)) {
      return false;
    }
    if (!this.isSameDepthAsSignatureCallee(node.callee)) {
      return false;
    }
    return deepEqual(this.purifiedCallee, this.purifyAst(node.callee));
  }
  isSameDepthAsSignatureCallee (ast) {
    const depth = this.signatureCalleeDepth;
    let currentDepth = 0;
    estraverse.traverse(ast, {
      keys: this.visitorKeys,
      enter: function (currentNode, parentNode) {
        const path = this.path();
        const pathDepth = path ? path.length : 0;
        if (currentDepth < pathDepth) {
          currentDepth = pathDepth;
        }
        if (depth < currentDepth) {
          this['break']();
        }
      }
    });
    return (depth === currentDepth);
  }
}

const toArgumentSignature = (argSignatureNode, idx) => {
  switch (argSignatureNode.type) {
    case syntax.Identifier:
      return {
        index: idx,
        name: argSignatureNode.name,
        kind: 'mandatory'
      };
    case syntax.ArrayExpression:
      return {
        index: idx,
        name: argSignatureNode.elements[0].name,
        kind: 'optional'
      };
    default:
      return null;
  }
};

const astDepth = (ast, visitorKeys) => {
  let maxDepth = 0;
  estraverse.traverse(ast, {
    keys: visitorKeys,
    enter: function (currentNode, parentNode) {
      const path = this.path();
      const pathDepth = path ? path.length : 0;
      if (maxDepth < pathDepth) {
        maxDepth = pathDepth;
      }
    }
  });
  return maxDepth;
};

const isCallExpression = (node) => node && node.type === syntax.CallExpression;
const isIdentifier = (node) => node.type === syntax.Identifier;
const isCalleeOfParent = (currentNode, parentNode) => parentNode &&
          currentNode &&
          parentNode.type === syntax.CallExpression &&
          parentNode.callee === currentNode;

const validateApiExpression = (callExpression) => {
  if (!callExpression || !callExpression.type) {
    throw new Error(notCallExprMessage);
  }
  if (callExpression.type !== syntax.CallExpression) {
    throw new Error(notCallExprMessage);
  }
  const names = {};
  callExpression.arguments.forEach((arg) => {
    const name = validateArg(arg);
    if (hasOwn.call(names, name)) {
      throw new Error(duplicatedArgMessage + name);
    } else {
      names[name] = name;
    }
  });
};

const validateArg = (arg) => {
  switch (arg.type) {
    case syntax.Identifier:
      return arg.name;
    case syntax.ArrayExpression:
      if (arg.elements.length !== 1) {
        throw new Error(invalidFormMessage);
      }
      const inner = arg.elements[0];
      if (inner.type !== syntax.Identifier) {
        throw new Error(invalidFormMessage);
      }
      return inner.name;
    default:
      throw new Error(invalidFormMessage);
  }
};

module.exports = CallMatcher;
