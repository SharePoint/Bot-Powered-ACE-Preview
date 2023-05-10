"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = y[op[0] & 2 ? "return" : op[0] ? "throw" : "next"]) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [0, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
Object.defineProperty(exports, "__esModule", { value: true });
var fs = require("fs");
var iconv = require("iconv-lite");
var jschardet = require("jschardet");
function read(path) {
    return __awaiter(this, void 0, void 0, function () {
        var stat, fd, result, buffer;
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0: return [4 /*yield*/, fileStat(path)];
                case 1:
                    stat = _a.sent();
                    fd = null;
                    _a.label = 2;
                case 2:
                    _a.trys.push([2, , 5, 8]);
                    return [4 /*yield*/, openFile(path, "r")];
                case 3:
                    fd = _a.sent();
                    buffer = new Buffer(stat.size);
                    return [4 /*yield*/, readFile(fd, buffer, 0, stat.size, 0)];
                case 4:
                    _a.sent();
                    result = decode(buffer);
                    return [3 /*break*/, 8];
                case 5:
                    if (!((fd !== null) && (fd !== undefined))) return [3 /*break*/, 7];
                    return [4 /*yield*/, closeFile(fd)];
                case 6:
                    _a.sent();
                    _a.label = 7;
                case 7: return [7 /*endfinally*/];
                case 8: return [2 /*return*/, result];
            }
        });
    });
}
exports.read = read;
function readSync(path) {
    var stat = fs.statSync(path);
    var fd = null;
    var result;
    try {
        fd = fs.openSync(path, "r");
        var buffer = new Buffer(stat.size);
        fs.readSync(fd, buffer, 0, stat.size, 0);
        result = decode(buffer);
    }
    finally {
        if ((fd !== null) && (fd !== undefined)) {
            fs.closeSync(fd);
        }
    }
    return result;
}
exports.readSync = readSync;
function decode(buffer) {
    // TODO: fallback for when confidence is too low? (pass it as "defaultEncoding" below)
    // TODO: this is decoding the whole file twice (once to get encoding name, then again to really decode... should just take a portion of it to get the encoding name)
    var encodingName = getEncodingName(buffer);
    return iconv.decode(buffer, encodingName, { stripBOM: true, addBOM: false, defaultEncoding: "utf-8" });
}
function getEncodingName(buffer) {
    // TODO: set min confidence?
    var result = jschardet.detect(buffer);
    return result.encoding;
}
// TODO: share these, or try fs-promise (or similar)
function fileStat(path) {
    return new Promise(function (resolve, reject) {
        fs.stat(path, function (err, stats) {
            if ((null !== err) && (undefined !== err)) {
                reject(err);
            }
            else {
                resolve(stats);
            }
        });
    });
}
function openFile(path, flags) {
    return new Promise(function (resolve, reject) {
        fs.open(path, flags, function (err, fd) {
            if ((null !== err) && (undefined !== err)) {
                reject(err);
            }
            else {
                resolve(fd);
            }
        });
    });
}
function readFile(fd, buffer, offset, length, position) {
    return new Promise(function (resolve, reject) {
        fs.read(fd, buffer, offset, length, position, function (err, bytesRead, buffer) {
            if ((null !== err) && (undefined !== err)) {
                reject(err);
            }
            else {
                resolve({ bytesRead: bytesRead, buffer: buffer });
            }
        });
    });
}
function closeFile(fd) {
    return new Promise(function (resolve, reject) {
        fs.close(fd, function (err) {
            if ((null !== err) && (undefined !== err)) {
                reject(err);
            }
            else {
                resolve();
            }
        });
    });
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicmVhZC10ZXh0LWZpbGUuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlcyI6WyIuLi9zcmMvcmVhZC10ZXh0LWZpbGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7OztBQUFBLHVCQUF5QjtBQUN6QixrQ0FBb0M7QUFFcEMsSUFBSSxTQUFTLEdBQUcsT0FBTyxDQUFDLFdBQVcsQ0FBQyxDQUFDO0FBRXJDLGNBQTJCLElBQVk7O2tCQUlsQyxFQUFFLEVBQ0YsTUFBTSxFQU1MLE1BQU07Ozt3QkFUQSxxQkFBTSxRQUFRLENBQUMsSUFBSSxDQUFDLEVBQUE7OzJCQUFwQixTQUFvQjt5QkFFZCxJQUFJOzs7O29CQUtmLHFCQUFNLFFBQVEsQ0FBQyxJQUFJLEVBQUUsR0FBRyxDQUFDLEVBQUE7O29CQUE5QixFQUFFLEdBQUcsU0FBeUIsQ0FBQzs2QkFFbEIsSUFBSSxNQUFNLENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQztvQkFDbEMscUJBQU0sUUFBUSxDQUFDLEVBQUUsRUFBRSxNQUFNLEVBQUUsQ0FBQyxFQUFFLElBQUksQ0FBQyxJQUFJLEVBQUUsQ0FBQyxDQUFDLEVBQUE7O29CQUEzQyxTQUEyQyxDQUFDO29CQUU1QyxNQUFNLEdBQUcsTUFBTSxDQUFDLE1BQU0sQ0FBQyxDQUFDOzs7eUJBSXBCLENBQUEsQ0FBQyxFQUFFLEtBQUssSUFBSSxDQUFDLElBQUksQ0FBQyxFQUFFLEtBQUssU0FBUyxDQUFDLENBQUEsRUFBbkMsd0JBQW1DO29CQUV0QyxxQkFBTSxTQUFTLENBQUMsRUFBRSxDQUFDLEVBQUE7O29CQUFuQixTQUFtQixDQUFDOzs7d0JBSXRCLHNCQUFPLE1BQU0sRUFBQzs7OztDQUNkO0FBekJELG9CQXlCQztBQUVELGtCQUF5QixJQUFZO0lBRXBDLElBQUksSUFBSSxHQUFHLEVBQUUsQ0FBQyxRQUFRLENBQUMsSUFBSSxDQUFDLENBQUM7SUFFN0IsSUFBSSxFQUFFLEdBQVcsSUFBSSxD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TUFBTSxFQUNOLE1BQU0sRUFDTixNQUFNLEVBQ04sUUFBUSxFQUNSLFVBQVUsR0FBRyxFQUFFLFNBQVMsRUFBRSxNQUFNO1lBRS9CLEVBQUUsQ0FBQyxDQUFDLENBQUMsSUFBSSxLQUFLLEdBQUcsQ0FBQyxJQUFJLENBQUMsU0FBUyxLQUFLLEdBQUcsQ0FBQyxDQUFDLENBQzFDLENBQUM7Z0JBQ0EsTUFBTSxDQUFDLEdBQUcsQ0FBQyxDQUFDO1lBQ2IsQ0FBQztZQUNELElBQUksQ0FDSixDQUFDO2dCQUNBLE9BQU8sQ0FBQyxFQUFFLFNBQVMsRUFBRSxTQUFTLEVBQUUsTUFBTSxFQUFFLE1BQU0sRUFBRSxDQUFDLENBQUM7WUFDbkQsQ0FBQztRQUNGLENBQUMsQ0FBQyxDQUFDO0lBQ0wsQ0FBQyxDQUFDLENBQUM7QUFDTCxDQUFDO0FBRUQsbUJBQW1CLEVBQVU7SUFFNUIsTUFBTSxDQUFDLElBQUksT0FBTyxDQUNqQixVQUFVLE9BQU8sRUFBRSxNQUFNO1FBRXhCLEVBQUUsQ0FBQyxLQUFLLENBQ1AsRUFBRSxFQUNGLFVBQVUsR0FBRztZQUVaLEVBQUUsQ0FBQyxDQUFDLENBQUMsSUFBSSxLQUFLLEdBQUcsQ0FBQyxJQUFJLENBQUMsU0FBUyxLQUFLLEdBQUcsQ0FBQyxDQUFDLENBQzFDLENBQUM7Z0JBQ0EsTUFBTSxDQUFDLEdBQUcsQ0FBQyxDQUFDO1lBQ2IsQ0FBQztZQUNELElBQUksQ0FDSixDQUFDO2dCQUNBLE9BQU8sRUFBRSxDQUFDO1lBQ1gsQ0FBQztRQUNGLENBQUMsQ0FBQyxDQUFDO0lBQ0wsQ0FBQyxDQUFDLENBQUM7QUFDTCxDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0ICogYXMgZnMgZnJvbSBcImZzXCI7XG5pbXBvcnQgKiBhcyBpY29udiBmcm9tIFwiaWNvbnYtbGl0ZVwiO1xuXG5sZXQganNjaGFyZGV0ID0gcmVxdWlyZShcImpzY2hhcmRldFwiKTtcblxuZXhwb3J0IGFzeW5jIGZ1bmN0aW9uIHJlYWQocGF0aDogc3RyaW5nKTogUHJvbWlzZTxzdHJpbmc+XG57XG5cdGxldCBzdGF0ID0gYXdhaXQgZmlsZVN0YXQocGF0aCk7XG5cblx0bGV0IGZkOiBudW1iZXIgPSBudWxsO1xuXHRsZXQgcmVzdWx0OiBzdHJpbmc7XG5cblx0dHJ5XG5cdHtcblx0XHRmZCA9IGF3YWl0IG9wZW5GaWxlKHBhdGgsIFwiclwiKTtcblxuXHRcdGxldCBidWZmZXIgPSBuZXcgQnVmZmVyKHN0YXQuc2l6ZSk7XG5cdFx0YXdhaXQgcmVhZEZpbGUoZmQsIGJ1ZmZlciwgMCwgc3RhdC5zaXplLCAwKTtcblxuXHRcdHJlc3VsdCA9IGRlY29kZShidWZmZXIpO1xuXHR9XG5cdGZpbmFsbHlcblx0e1xuXHRcdGlmICgoZmQgIT09IG51bGwpICYmIChmZCAhPT0gdW5kZWZpbmVkKSlcblx0XHR7XG5cdFx0XHRhd2FpdCBjbG9zZUZpbGUoZmQpO1xuXHRcdH1cblx0fVxuXG5cdHJldHVybiByZXN1bHQ7XG59XG5cbmV4cG9ydCBmdW5jdGlvbiByZWFkU3luYyhwYXRoOiBzdHJpbmcpOiBzdHJpbmdcbntcblx0bGV0IHN0YXQgPSBmcy5zdGF0U3luYyhwYXRoKTtcblxuXHRsZXQgZmQ6IG51bWJlciA9IG51bGw7XG5cdGxldCByZXN1bHQ6IHN0cmluZztcblxuXHR0cnlcblx0e1xuXHRcdGZkID0gZnMub3BlblN5bmMocGF0aCwgXCJyXCIpO1xuXG5cdFx0bGV0IGJ1ZmZlciA9IG5ldyBCdWZmZXIoc3RhdC5zaXplKTtcblx0XHRmcy5yZWFkU3luYyhmZCwgYnVmZmVyLCAwLCBzdGF0LnNpemUsIDApO1xuXG5cdFx0cmVzdWx0ID0gZGVjb2RlKGJ1ZmZlcik7XG5cdH1cblx0ZmluYWxseVxuXHR7XG5cdFx0aWYgKChmZCAhPT0gbnVsbCkgJiYgKGZkICE9PSB1bmRlZmluZWQpKVxuXHRcdHtcblx0XHRcdGZzLmNsb3NlU3luYyhmZCk7XG5cdFx0fVxuXHR9XG5cblx0cmV0dXJuIHJlc3VsdDtcbn1cblxuZnVuY3Rpb24gZGVjb2RlKGJ1ZmZlcjogQnVmZmVyKSA6IHN0cmluZ1xue1xuXHQvLyBUT0RPOiBmYWxsYmFjayBmb3Igd2hlbiBjb25maWRlbmNlIGlzIHRvbyBsb3c/IChwYXNzIGl0IGFzIFwiZGVmYXVsdEVuY29kaW5nXCIgYmVsb3cpXG5cdC8vIFRPRE86IHRoaXMgaXMgZGVjb2RpbmcgdGhlIHdob2xlIGZpbGUgdHdpY2UgKG9uY2UgdG8gZ2V0IGVuY29kaW5nIG5hbWUsIHRoZW4gYWdhaW4gdG8gcmVhbGx5IGRlY29kZS4uLiBzaG91bGQganVzdCB0YWtlIGEgcG9ydGlvbiBvZiBpdCB0byBnZXQgdGhlIGVuY29kaW5nIG5hbWUpXG5cdGxldCBlbmNvZGluZ05hbWUgPSBnZXRFbmNvZGluZ05hbWUoYnVmZmVyKTtcblx0cmV0dXJuIGljb252LmRlY29kZShidWZmZXIsIGVuY29kaW5nTmFtZSwge3N0cmlwQk9NOiB0cnVlLCBhZGRCT006IGZhbHNlLCBkZWZhdWx0RW5jb2Rpbmc6IFwidXRmLThcIn0pO1xufVxuXG5mdW5jdGlvbiBnZXRFbmNvZGluZ05hbWUoYnVmZmVyOiBCdWZmZXIpIDogc3RyaW5nXG57XG5cdC8vIFRPRE86IHNldCBtaW4gY29uZmlkZW5jZT9cblx0bGV0IHJlc3VsdCA9IGpzY2hhcmRldC5kZXRlY3QoYnVmZmVyKTtcblx0cmV0dXJuIHJlc3VsdC5lbmNvZGluZztcbn1cblxuLy8gVE9ETzogc2hhcmUgdGhlc2UsIG9yIHRyeSBmcy1wcm9taXNlIChvciBzaW1pbGFyKVxuZnVuY3Rpb24gZmlsZVN0YXQocGF0aDogc3RyaW5nIHwgQnVmZmVyKTogUHJvbWlzZTxmcy5TdGF0cz5cbntcblx0cmV0dXJuIG5ldyBQcm9taXNlPGZzLlN0YXRzPihcblx0XHRmdW5jdGlvbiAocmVzb2x2ZSwgcmVqZWN0KVxuXHRcdHtcblx0XHRcdGZzLnN0YXQoXG5cdFx0XHRcdHBhdGgsXG5cdFx0XHRcdGZ1bmN0aW9uIChlcnIsIHN0YXRzKVxuXHRcdFx0XHR7XG5cdFx0XHRcdFx0aWYgKChudWxsICE9PSBlcnIpICYmICh1bmRlZmluZWQgIT09IGVycikpXG5cdFx0XHRcdFx0e1xuXHRcdFx0XHRcdFx0cmVqZWN0KGVycik7XG5cdFx0XHRcdFx0fVxuXHRcdFx0XHRcdGVsc2Vcblx0XHRcdFx0XHR7XG5cdFx0XHRcdFx0XHRyZXNvbHZlKHN0YXRzKTtcblx0XHRcdFx0XHR9XG5cdFx0XHRcdH0pO1xuXHRcdH0pO1xufVxuXG5mdW5jdGlvbiBvcGVuRmlsZShwYXRoOiBzdHJpbmcgfCBCdWZmZXIsIGZsYWdzOiBzdHJpbmcgfCBudW1iZXIpOiBQcm9taXNlPG51bWJlcj5cbntcblx0cmV0dXJuIG5ldyBQcm9taXNlPG51bWJlcj4oXG5cdFx0ZnVuY3Rpb24gKHJlc29sdmUsIHJlamVjdClcblx0XHR7XG5cdFx0XHRmcy5vcGVuKFxuXHRcdFx0XHRwYXRoLFxuXHRcdFx0XHRmbGFncyxcblx0XHRcdFx0ZnVuY3Rpb24gKGVyciwgZmQpXG5cdFx0XHRcdHtcblx0XHRcdFx0XHRpZiAoKG51bGwgIT09IGVycikgJiYgKHVuZGVmaW5lZCAhPT0gZXJyKSlcblx0XHRcdFx0XHR7XG5cdFx0XHRcdFx0XHRyZWplY3QoZXJyKTtcblx0XHRcdFx0XHR9XG5cdFx0XHRcdFx0ZWxzZVxuXHRcdFx0XHRcdHtcblx0XHRcdFx0XHRcdHJlc29sdmUoZmQpO1xuXHRcdFx0XHRcdH1cblx0XHRcdFx0fSk7XG5cdFx0fSk7XG59XG5cbmludGVyZmFjZSBSZWFkRmlsZVJlc3VsdFxue1xuXHRieXRlc1JlYWQ6IG51bWJlcjtcblx0YnVmZmVyOiBCdWZmZXI7XG59XG5cbmZ1bmN0aW9uIHJlYWRGaWxlKGZkOiBudW1iZXIsIGJ1ZmZlcjogQnVmZmVyLCBvZmZzZXQ6IG51bWJlciwgbGVuZ3RoOiBudW1iZXIsIHBvc2l0aW9uOiBudW1iZXIpOiBQcm9taXNlPFJlYWRGaWxlUmVzdWx0Plxue1xuXHRyZXR1cm4gbmV3IFByb21pc2U8UmVhZEZpbGVSZXN1bHQ+KFxuXHRcdGZ1bmN0aW9uIChyZXNvbHZlLCByZWplY3QpXG5cdFx0e1xuXHRcdFx0ZnMucmVhZChcblx0XHRcdFx0ZmQsXG5cdFx0XHRcdGJ1ZmZlcixcblx0XHRcdFx0b2Zmc2V0LFxuXHRcdFx0XHRsZW5ndGgsXG5cdFx0XHRcdHBvc2l0aW9uLFxuXHRcdFx0XHRmdW5jdGlvbiAoZXJyLCBieXRlc1JlYWQsIGJ1ZmZlcilcblx0XHRcdFx0e1xuXHRcdFx0XHRcdGlmICgobnVsbCAhPT0gZXJyKSAmJiAodW5kZWZpbmVkICE9PSBlcnIpKVxuXHRcdFx0XHRcdHtcblx0XHRcdFx0XHRcdHJlamVjdChlcnIpO1xuXHRcdFx0XHRcdH1cblx0XHRcdFx0XHRlbHNlXG5cdFx0XHRcdFx0e1xuXHRcdFx0XHRcdFx0cmVzb2x2ZSh7IGJ5dGVzUmVhZDogYnl0ZXNSZWFkLCBidWZmZXI6IGJ1ZmZlciB9KTtcblx0XHRcdFx0XHR9XG5cdFx0XHRcdH0pO1xuXHRcdH0pO1xufVxuXG5mdW5jdGlvbiBjbG9zZUZpbGUoZmQ6IG51bWJlcik6IFByb21pc2U8dm9pZD5cbntcblx0cmV0dXJuIG5ldyBQcm9taXNlPHZvaWQ+KFxuXHRcdGZ1bmN0aW9uIChyZXNvbHZlLCByZWplY3QpXG5cdFx0e1xuXHRcdFx0ZnMuY2xvc2UoXG5cdFx0XHRcdGZkLFxuXHRcdFx0XHRmdW5jdGlvbiAoZXJyKVxuXHRcdFx0XHR7XG5cdFx0XHRcdFx0aWYgKChudWxsICE9PSBlcnIpICYmICh1bmRlZmluZWQgIT09IGVycikpXG5cdFx0XHRcdFx0e1xuXHRcdFx0XHRcdFx0cmVqZWN0KGVycik7XG5cdFx0XHRcdFx0fVxuXHRcdFx0XHRcdGVsc2Vcblx0XHRcdFx0XHR7XG5cdFx0XHRcdFx0XHRyZXNvbHZlKCk7XG5cdFx0XHRcdFx0fVxuXHRcdFx0XHR9KTtcblx0XHR9KTtcbn0iXX0=