module Cb {
    String.prototype.isBlank = function(this : string) {
        return this.trim().length === 0;
    }
}
