const path = require("path");

module.exports = (invert = false) => {
  const pkg = require(path.join(process.cwd(), "package.json"));
  process.exit(pkg.private === true ? (invert ? 1 : 0) : (invert ? 0 : 1));
}