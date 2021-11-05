"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Member = void 0;
var Member = /** @class */ (function () {
    function Member(localPeer, remotePeer, localStream, remoteStream, audioState, videoState, accountImage, nickName) {
        if (localPeer === void 0) { localPeer = null; }
        if (remotePeer === void 0) { remotePeer = null; }
        if (localStream === void 0) { localStream = null; }
        if (remoteStream === void 0) { remoteStream = null; }
        if (audioState === void 0) { audioState = true; }
        if (videoState === void 0) { videoState = true; }
        if (accountImage === void 0) { accountImage = ""; }
        if (nickName === void 0) { nickName = ""; }
        this.localPeer = localPeer;
        this.remotePeer = remotePeer;
        this.localStream = localStream;
        this.remoteStream = remoteStream;
        this.audioState = audioState;
        this.videoState = videoState;
        this.accountImage = accountImage;
        this.nickName = nickName;
    }
    return Member;
}());
exports.Member = Member;
//# sourceMappingURL=member.js.map