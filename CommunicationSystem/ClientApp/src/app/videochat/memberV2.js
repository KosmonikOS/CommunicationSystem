"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Member = void 0;
var Member = /** @class */ (function () {
    function Member(_myself, _localPeer, _remotePeer, _localStream, _remoteStream, _audioState, _videoState, _accountImage, _nickName, state) {
        if (_myself === void 0) { _myself = false; }
        if (_localPeer === void 0) { _localPeer = null; }
        if (_remotePeer === void 0) { _remotePeer = null; }
        if (_localStream === void 0) { _localStream = null; }
        if (_remoteStream === void 0) { _remoteStream = null; }
        if (_audioState === void 0) { _audioState = true; }
        if (_videoState === void 0) { _videoState = true; }
        if (_accountImage === void 0) { _accountImage = ""; }
        if (_nickName === void 0) { _nickName = ""; }
        if (state === void 0) { state = false; }
        this._myself = _myself;
        this._localPeer = _localPeer;
        this._remotePeer = _remotePeer;
        this._localStream = _localStream;
        this._remoteStream = _remoteStream;
        this._audioState = _audioState;
        this._videoState = _videoState;
        this._accountImage = _accountImage;
        this._nickName = _nickName;
        this.state = state;
    }
    Object.defineProperty(Member.prototype, "myself", {
        get: function () {
            return this._myself;
        },
        set: function (value) {
            this._myself = value;
            this.state = !this.state;
        },
        enumerable: false,
        configurable: true
    });
    Object.defineProperty(Member.prototype, "localPeer", {
        get: function () {
            return this._localPeer;
        },
        set: function (value) {
            this._localPeer = value;
            this.state = !this.state;
        },
        enumerable: false,
        configurable: true
    });
    Object.defineProperty(Member.prototype, "remotePeer", {
        get: function () {
            return this._remotePeer;
        },
        set: function (value) {
            this._remotePeer = value;
            this.state = !this.state;
        },
        enumerable: false,
        configurable: true
    });
    Object.defineProperty(Member.prototype, "localStream", {
        get: function () {
            return this._localStream;
        },
        set: function (value) {
            this._localStream = value;
            this.state = !this.state;
        },
        enumerable: false,
        configurable: true
    });
    Object.defineProperty(Member.prototype, "remoteStream", {
        get: function () {
            return this._remoteStream;
        },
        set: function (value) {
            this._remoteStream = value;
            this.state = !this.state;
        },
        enumerable: false,
        configurable: true
    });
    Object.defineProperty(Member.prototype, "audioState", {
        get: function () {
            return this._audioState;
        },
        set: function (value) {
            this._audioState = value;
            this.state = !this.state;
        },
        enumerable: false,
        configurable: true
    });
    Object.defineProperty(Member.prototype, "videoState", {
        get: function () {
            return this._videoState;
        },
        set: function (value) {
            this._videoState = value;
            this.state = !this.state;
        },
        enumerable: false,
        configurable: true
    });
    Object.defineProperty(Member.prototype, "accountImage", {
        get: function () {
            return this._accountImage;
        },
        set: function (value) {
            this._accountImage = value;
            this.state = !this.state;
        },
        enumerable: false,
        configurable: true
    });
    Object.defineProperty(Member.prototype, "nickName", {
        get: function () {
            return this._nickName;
        },
        set: function (value) {
            this._nickName = value;
            this.state = !this.state;
        },
        enumerable: false,
        configurable: true
    });
    return Member;
}());
exports.Member = Member;
//# sourceMappingURL=member.js.map