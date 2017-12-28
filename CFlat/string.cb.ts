module Cb {
    String.prototype.isBlank = function(this : string) {
        if(!this) return true;
        return this.trim().length == 0;
    }
}
