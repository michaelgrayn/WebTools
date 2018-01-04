export module Cb {
    String.prototype.isBlank = function(this : string) {
        return this.trim().length === 0;
    }

    Array.prototype.add = function(this : Array<any>, element: any) {
        this.push(element);
    }

    Array.prototype.remove = function(this : Array<any>, element?: any) {
        let index = this.length - 1;
        if(element) index = this.indexOf(element);
        this.removeAt(index);
    }

    Array.prototype.removeAt = function(this : Array<any>, index: number) {
        this.splice(index, 1);
    }

    Array.prototype.first = function(this: Array<any>) {
        return this[0];
    }

    Array.prototype.last = function(this: Array<any>) {
        return this[this.length - 1];
    }
}
