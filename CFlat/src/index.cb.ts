export module Cb {
    String.prototype.isBlank = function(this : string) {
        return this.trim().length === 0;
    }

    Array.prototype.pop = function(this : Array<any>, ...items: any[]) {
        for (const item of items) {
            this.popAt(this.indexOf(item));
        }
        return this.length;
    }

    Array.prototype.popAt = function(this : Array<any>, index: number) {
        return this.splice(index, 1).first();
    }

    Array.prototype.first = function(this: Array<any>) {
        return this[0];
    }

    Array.prototype.last = function(this: Array<any>) {
        return this[this.length - 1];
    }
}
