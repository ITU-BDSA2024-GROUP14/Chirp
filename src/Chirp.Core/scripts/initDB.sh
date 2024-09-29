#!/usr/bin/env bash
sqlite3 $TMPDIR/chirp.db < ../data/schema.sql
sqlite3 $TMPDIR/chirp.db < ../data/dump.sql