export interface Patch {
  op: 'add' | 'remove' | 'replace' | 'move' | 'copy' | 'test',
  path: string,
  value: any
}
