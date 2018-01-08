export module Cb {
    String.prototype.isBlank = function(this : string) {
        return this.trim().length === 0;
    }

    Array.prototype.any = function(this: Array<any>) {
        return this.length > 0;
    }

    Array.prototype.first = function(this: Array<any>) {
        return this[0];
    }

    Array.prototype.last = function(this: Array<any>) {
        return this[this.length - 1];
    }

    Array.prototype.popElements = function(this: Array<any>, ...items: any[]) {
        for (const item of items) {
            let index = this.indexOf(item);
            while(index >= 0) {
                this.spliceAt(index);
                index = this.indexOf(item);
            }
        }
        return this.length;
    }

    Array.prototype.spliceAt = function(this: Array<any>, index: number) {
        return this.splice(index, 1).first();
    }
}
