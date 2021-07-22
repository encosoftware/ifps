import { Input, OnDestroy, Renderer2, ElementRef, Optional, Directive } from '@angular/core';
import { Subject } from 'rxjs';
import { CdkTreeNode, CdkTree } from '@angular/cdk/tree';
import { Directionality } from '@angular/cdk/bidi';
import { takeUntil } from 'rxjs/operators';
import { coerceNumberProperty } from '@angular/cdk/coercion';

const cssUnitPattern = /([A-Za-z%]+)$/;

@Directive({
  selector: '[butorMaterialTreePadding]'
})
export class MaterialTreePaddingDirective<T> implements OnDestroy {
  private _currentPadding: string | null;

  /** Subject that emits when the component has been destroyed. */
  private _destroyed = new Subject<void>();

  /** CSS units used for the indentation value. */
  indentUnits = 'rem';

  @Input('butorcdkTreeNodePadding')
  get level(): number { return this._level; }
  set level(value: number) {
    this._level = coerceNumberProperty(value);
    this._setPadding();
    this._setFontWeight();
  }
  _level: number;

  @Input('butorcdkTreeNodePaddingIndent')
  get indent(): number | string { return this._indent; }
  set indent(indent: number | string) {
    let value = indent;
    let units = 'rem';
    if (typeof indent === 'string') {
      const parts = indent.split(cssUnitPattern);
      value = parts[0];
      units = parts[1] || units;
    }

    this.indentUnits = units;
    this._indent = coerceNumberProperty(value);
    this._setPadding();
    this._setFontWeight();
  }
  _indent: number = 3;

  constructor(
    private _treeNode: CdkTreeNode<T>,
    private _tree: CdkTree<T>,
    private _renderer: Renderer2,
    private _element: ElementRef<HTMLElement>,
    @Optional() private _dir: Directionality) {
    this._setPadding();
    this._setFontWeight();
    if (_dir) {
      _dir.change.pipe(takeUntil(this._destroyed)).subscribe(() => {
        this._setPadding(true);
        this._setFontWeight(true);
      });
    }

    _treeNode._dataChanges.subscribe(() => {
      this._setPadding();
      this._setFontWeight();
    });
  }

  ngOnDestroy() {
    this._destroyed.next();
    this._destroyed.complete();
  }

  _paddingIndent(): string | null {
    const nodeLevel = (this._treeNode.data && this._tree.treeControl.getLevel)
      ? this._tree.treeControl.getLevel(this._treeNode.data)
      : null;
    const level = this._level || nodeLevel;

    return level ? `${(2.5 * level) + this._indent}${this.indentUnits}` : null;
  }

  _setPadding(forceChange = false) {
    const padding = this._paddingIndent();
    if (padding !== this._currentPadding || forceChange) {
      const element = this._element.nativeElement;
      const paddingProp = this._dir && this._dir.value === 'rtl' ? 'paddingRight' : 'paddingLeft';
      const resetProp = paddingProp === 'paddingLeft' ? 'paddingRight' : 'paddingLeft';
      this._renderer.setStyle(element, paddingProp, padding);
      this._renderer.setStyle(element, resetProp, null);
      this._currentPadding = padding;
    }
  }

  _fontWeightIndent(): string | null {
    const nodeLevel = (this._treeNode.data && this._tree.treeControl.getLevel)
      ? this._tree.treeControl.getLevel(this._treeNode.data)
      : null;
    const level = this._level || nodeLevel;
    return (!(level === 0 || level === null)) ? '400' : '900';
  }

  _setFontWeight(forceChange = false) {
    const font = this._fontWeightIndent();
    if (font === '400' || forceChange) {
      const element = this._element.nativeElement;
      const fontProp = this._dir && this._dir.value === 'rtl' ? 'fontWeight' : 'fontWeight';
      this._renderer.setStyle(element, fontProp, font);
    }
  }


}


